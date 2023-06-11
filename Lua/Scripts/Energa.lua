local Time = require("Time")
import("System.Text.RegularExpressions")

Notifier.interval = Time.hours(1)
function Notifier.update()
    -- Notifier.messageBox("Message from Lua", "Title")
    local success, result = Notifier.fetch("https://energa-operator.pl/__system/api/document/EXTERNAL_DOCUMENT-11")

    if success then
        local match = Regex.Match(result, "D[ęe]bowa", RegexOptions.IgnoreCase)
        if (match.Success) then
            Notifier.showNotification(Time.seconds(10), "ENERGA: Przerwa w dostawie prądu", "Strona: https://energa-operator.pl/uslugi/awarie-i-wylaczenia/wylaczenia-biezace")
        end
    else
        Notifier.showNotification(Time.seconds(10), "ENERGA: sprawdzenie bieżących wyłączeń", "Nie udało się połączyć z serwisami Energi w celu sprawdzenia bieżących wyłączeń")
    end
end
