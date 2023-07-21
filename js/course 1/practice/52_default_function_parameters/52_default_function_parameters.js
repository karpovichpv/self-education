function weatherForecast(city, weather) {
    weather = weather == undefined
        ? 'Excellent weather'
        : weather 
    return `Weather forecast for city ${city}: ${weather}`
}

function weatherForecast(city, weather = 'Excellent weather') {
    return `Weather forecast for city ${city}: ${weather}`
}

console.log(weatherForecast('Dubai', 'Sunny'))

console.log(weatherForecast('London', 'Small rain'))

console.log(weatherForecast('Paris'))

console.log(weatherForecast('Miami', ''))

console.log(weatherForecast('Las Vegas', undefined))