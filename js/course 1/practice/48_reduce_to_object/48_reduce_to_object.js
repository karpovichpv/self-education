function quantitiesByCategories(array) {
    return array.reduce((qtysByCategories, product) => {
        qtysByCategories[product.category]
            = (qtysByCategories[product.category] || 0)
            + product.quantity
        return qtysByCategories
    }, {})
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
        title: 'Watches',
        price: 8,
        quantity: 6,
        category: 'Accessories'
    },
]

console.log(quantitiesByCategories(inputProducts))