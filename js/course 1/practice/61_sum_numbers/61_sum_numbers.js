function sumNumbers() {
    let sum = 0
    for (let index = 0; index < arguments.length; index++) {
        const element = arguments[index];
        sum+=element
    }

    console.log('sum of all arguments is ', sum)
    return sum
}

sumNumbers(1, 3)

sumNumbers(10, 20, 5)

sumNumbers(2, 5, 80, 1, 10, 12)