const cities = ['Warsaw', 'Poznan', 'Calgary']

const cityMessage = (cityName, index) =>
    console.log(`${cityName} is at index ${index} in the cities array`)

for (let i = 0; i < cities.length; i++)
    cityMessage(cities[i], i)

cities.forEach((value, index) => cityMessage(value, index))