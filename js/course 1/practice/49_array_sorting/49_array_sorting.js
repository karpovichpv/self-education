const sortProductsByPrice = (products) => {
    const sortedArray = [...products]
    sortedArray.sort((a, b) => a.price - b.price)
    return sortedArray
}

const inputProducts = [
    {
        title: 'Phone case',
        price: 23,
        quantity: 2,
        category: 'Accessories'
    },
    {
        title: 'Android phone',
        price: 150,
        quantity: 1,
        category: 'Phones'
    },
    {
        title: 'Headphones',
        price: 78,
        quantity: 1,
        category: 'Accessories'
    },
    {
        title: 'Sport Watch',
        price: 55,
        quantity: 2,
        category: 'Watches'
    },
]

console.log(sortProductsByPrice(inputProducts))
console.log(inputProducts)