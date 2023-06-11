using NLua;
using NLua.Exceptions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Timer = System.Windows.Forms.Timer;

namespace Notifier
{
    public partial class Notifier : Form
    {
        public Notifier()
        {
            InitializeComponent();
            ResetScripts();

            displayLuaErrorsItem.Text = $"Display Lua errors {(displayLuaErrors ? "ON" : "OFF")}";
            silentModeItem.Text = $"Silent Mode {(silentMode ? "ON" : "OFF")}";
        }

        static async Task<string> FetchAsync(string url)
        {
            using var client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        static (bool success, string websiteContents) LuaFetch(string url)
        {
            var asyncDownload = Task.Run(() => FetchAsync(url));
            try
            {
                asyncDownload.Wait();
            }
            catch (Exception e)
            {
                return (success: false, websiteContents: e.Message);
            }
            return (success: true, websiteContents: asyncDownload.Result);
        }

        void LuaShowNotification(int timeout, string title, string text)
        {
            if (silentMode) return;
            notifyIcon.ShowBalloonTip(timeout, title, text, ToolTipIcon.Info);
        }

        void LuaMessageBox(string text, string caption)
        {
            if (silentMode) return;
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string basePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\");
        private Dictionary<string, LuaScript> scripts = new();
        private List<Timer> scriptTimers = new();
        private int defaultScriptInterval = TimeSpan.FromSeconds(2.5).Milliseconds;
        private bool displayLuaErrors = false;
        private bool silentMode = false;

        void ResetScripts()
        {
            scripts.Clear();
            foreach (Timer timer in scriptTimers)
                timer.Stop();
            scriptTimers.Clear();

            try
            {
                foreach (string file in Directory.EnumerateFiles(Path.Combine(basePath, "Lua", "Scripts"), "*.lua"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    string contents = File.ReadAllText(file);

                    var luaScript = new LuaScript(name);
                    InitializeLuaEnvironment(luaScript.environment);

                    try
                    {
                        luaScript.environment.DoString(contents);
                        scripts.Add(name, luaScript);
                        CreateScriptTimer(name, RunScriptUpdate);
                    }
                    catch (LuaScriptException e)
                    {
                        if (displayLuaErrors && !silentMode)
                            MessageBox.Show($"An error occurred in script \"{name}.lua\":\n\n{e.Message}\n\n{e.Source}",
                                "Notifier Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception)
            {
                if (!silentMode)
                    MessageBox.Show($"An error occurred while looking for Lua scripts",
                        "Notifier Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }
        }

        void CreateScriptTimer(string scriptName, EventHandler callback)
        {
            Timer timer = new()
            {
                Tag = scriptName
            };
            timer.Tick += new EventHandler(callback);
            timer.Start();

            scriptTimers.Add(timer);
        }

        void InitializeLuaEnvironment(Lua lua)
        {
            lua.State.Encoding = Encoding.UTF8;

            string librariesPath = $@"{basePath}Lua\\Libraries\\?.lua;{basePath}Lua\\Libraries\\?\\init.lua;{basePath}Lua\\Libraries\\?.dll;";
            lua.DoString($@"package.path = '{librariesPath}' .. package.path");

            lua.NewTable("Notifier");
            lua.RegisterFunction("Notifier.fetch", typeof(Notifier).GetMethod("LuaFetch", BindingFlags.Static | BindingFlags.NonPublic));
            lua.RegisterFunction("Notifier.showNotification", this, typeof(Notifier).GetMethod("LuaShowNotification", BindingFlags.Instance | BindingFlags.NonPublic));
            lua.RegisterFunction("Notifier.messageBox", this, typeof(Notifier).GetMethod("LuaMessageBox", BindingFlags.Instance | BindingFlags.NonPublic));

            lua.LoadCLRPackage();
        }

        private void RunScriptUpdate(object? sender, EventArgs args)
        {
            if (sender is Timer timer && timer.Tag is string scriptName)
            {
                timer.Interval = GetScriptInterval(scriptName);
                if (GetScriptUpdateCallback(scriptName) is LuaFunction callback)
                {
                    try
                    {
                        callback.Call();
                    }
                    catch (LuaScriptException e)
                    {
                        if (displayLuaErrors && !silentMode)
                            MessageBox.Show($"An error occurred in script \"{scriptName}.lua\":\n\n{e.Message}\n\n{e.Source}",
                                "Notifier Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private int GetScriptInterval(string scriptName)
        {
            int interval = defaultScriptInterval;
            if (scripts[scriptName].environment["Notifier"] is LuaTable settings)
            {
                if (settings["interval"] is Int64 intInterval)
                    interval = (int)intInterval;
            }
            return interval;
        }

        private LuaFunction? GetScriptUpdateCallback(string scriptName)
        {
            if (scripts[scriptName].environment["Notifier"] is LuaTable settings)
            {
                if (settings["update"] is LuaFunction callback)
                    return callback;
            }
            return null;
        }

        private void displayLuaErrorsItem_Click(object sender, EventArgs e)
        {
            displayLuaErrors = !displayLuaErrors;
            displayLuaErrorsItem.Text = $"Display Lua errors {(displayLuaErrors ? "ON" : "OFF")}";
        }

        private void silentModeItem_Click(object sender, EventArgs e)
        {
            silentMode = !silentMode;
            silentModeItem.Text = $"Silent Mode {(silentMode ? "ON" : "OFF")}";
        }

        private void resetItem_Click(object sender, EventArgs e)
        {
            ResetScripts();
        }

        private void exitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Notifier_Load(object sender, EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            Opacity = 0;
        }
    }

    public class LuaScript
    {
        public string name;
        public Lua environment;

        public LuaScript(string name = "", Lua? environment = null)
        {
            this.name = name;
            this.environment = environment ?? new Lua();
        }
    }
}
