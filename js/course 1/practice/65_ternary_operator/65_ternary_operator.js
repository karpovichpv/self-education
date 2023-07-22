function isNumber(a) {
    return typeof a === 'number'
        ? `${a} - это число`
        : `${a} - это не число`
}

console.log(isNumber(10))

console.log(isNumber('Привет'))

console.log(isNumber(true))