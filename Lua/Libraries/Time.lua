local Time = {}

function Time.seconds(amount) return amount * 1000 end
function Time.minutes(amount) return amount * 60 * 1000 end
function Time.hours(amount) return amount * 60 * 60 * 1000 end

return Time
