const cars = [
    {brand : 'Honda', price : 13000},
    {brand : 'Rolls-Royce', price : 123000},
]

function carInfo({brand,price}){
    return `Price of automobile ${brand} - ${price} and this is ${price <= 20000? 'cheap' : 'expensive'} car`
}

cars.forEach((element) => console.log(carInfo(element)))