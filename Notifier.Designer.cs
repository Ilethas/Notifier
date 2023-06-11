namespace Notifier
{
    partial class Notifier
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notifier));
            settingsMenu = new ContextMenuStrip(components);
            displayLuaErrorsItem = new ToolStripMenuItem();
            silentModeItem = new ToolStripMenuItem();
            resetItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            exitItem = new ToolStripMenuItem();
            notifyIcon = new NotifyIcon(components);
            settingsMenu.SuspendLayout();
            SuspendLayout();
            // 
            // settingsMenu
            // 
            settingsMenu.Items.AddRange(new ToolStripItem[] { displayLuaErrorsItem, silentModeItem, resetItem, toolStripMenuItem1, exitItem });
            settingsMenu.Name = "contextMenu";
            settingsMenu.Size = new Size(181, 120);
            // 
            // displayLuaErrorsItem
            // 
            displayLuaErrorsItem.Image = Properties.Resources.Warning;
            displayLuaErrorsItem.Name = "displayLuaErrorsItem";
            displayLuaErrorsItem.Size = new Size(180, 22);
            displayLuaErrorsItem.Text = "Display Lua errors";
            displayLuaErrorsItem.ToolTipText = "Whether or not errors in Lua scripts should be displayed to the user in message boxes";
            displayLuaErrorsItem.Click += displayLuaErrorsItem_Click;
            // 
            // silentModeItem
            // 
            silentModeItem.Image = Properties.Resources.Silent_Mode;
            silentModeItem.Name = "silentModeItem";
            silentModeItem.Size = new Size(180, 22);
            silentModeItem.Text = "Silent Mode";
            silentModeItem.ToolTipText = "No notifications and error messages are displayed while Silent Mode is active";
            silentModeItem.Click += silentModeItem_Click;
            // 
            // resetItem
            // 
            resetItem.Image = Properties.Resources.Reset;
            resetItem.Name = "resetItem";
            resetItem.Size = new Size(180, 22);
            resetItem.Text = "Reset";
            resetItem.ToolTipText = "Reload all Lua scripts from the Scripts folder";
            resetItem.Click += resetItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // exitItem
            // 
            exitItem.Image = Properties.Resources.Exit;
            exitItem.Name = "exitItem";
            exitItem.Size = new Size(180, 22);
            exitItem.Text = "Exit";
            exitItem.ToolTipText = "Exit the application";
            exitItem.Click += exitItem_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = settingsMenu;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "Notifier";
            notifyIcon.Visible = true;
            // 
            // Notifier
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 161);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Notifier";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Notifier";
            Load += Notifier_Load;
            settingsMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip settingsMenu;
        private ToolStripMenuItem exitItem;
        private NotifyIcon notifyIcon;
        private ToolStripMenuItem displayLuaErrorsItem;
        private ToolStripMenuItem resetItem;
        private ToolStripMenuItem silentModeItem;
        private ToolStripSeparator toolStripMenuItem1;
    }
}