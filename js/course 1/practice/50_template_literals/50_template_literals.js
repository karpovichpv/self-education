const moreOrLess = (num) => {
    if (num < 10) 
        return 'less'

    return 'more'
}

const templateLiteral = (num) => {
    return `Number ${num}
    this number is ${moreOrLess(num)} 10.
    Square root from this number is - ${Math.sqrt(num)}`
}

const myNumber = 9
console.log(templateLiteral(myNumber))


const myAnotherNumber = 25
console.log(templateLiteral(myAnotherNumber))