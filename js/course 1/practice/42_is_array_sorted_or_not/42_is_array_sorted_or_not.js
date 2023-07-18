const a = [5, 'abc', 10, 1]
const b = [4, 10, 14, 25, 25, 50]
const c = [150, 132, 80, 40]
const d = [15, 26, 10, 23, 85]

const hasStrings = (array) => {
    if (array.find((element) => typeof element !== 'number'))
        return true
    return false
}

const isSortedByAscending = (array) => {
    const sortedArray = [...array]
   sortedArray= sortedArray.sort()

    for (let i = 0; i < array.length; i++) {
        if (array[i] != sortedArray[i])
            return false
    }
    return true
}

const isSortedByDescending = (array) => {
    const sortedArray = [...array]
    sortedArray.sort()
    sortedArray.reverse()

    for (let i = 0; i < array.length; i++) {
        if (array[i] != sortedArray[i])
            return false
    }

    return true
}

const checkArray = (array) => {
    let messages = []
    if (hasStrings(array))
        messages.push('array has strings')
    if (isSortedByAscending(array))
        messages.push('array is sorted by ascending')
    if (isSortedByDescending(array))
        messages.push('array is sorted by descending')

    messages.forEach((el) => console.log(el))
}

console.log(checkArray(a))
console.log(checkArray(b))
console.log(checkArray(c))
console.log(checkArray(d))