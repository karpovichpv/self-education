let person = {
    _id: '5ad8cefcc0971792dacb3f1f',
    index: 4,
    processed: false,
    cart: ['item1', 'item2', 'item3'],
    email: 'jhon@yahoo.com',
    name: 'Jhon',
    cartId: 452,
}

//delete person._id
//delete person.processed
//delete person.cart

{
    let _id, processed, cart;
    ({ _id, processed, cart, ...person } = person)
}

console.log(person)