function square(a) {
    if (a === undefined) {
        throw Error('Function "square" can not be executed without arguments')
    }
    console.log(a * a);
}

square(10)

square()