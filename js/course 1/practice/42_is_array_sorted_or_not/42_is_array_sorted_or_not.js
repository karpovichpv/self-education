const a = [5, 'abc', 10, 1]
const b = [4, 10, 14, 25, 25, 50]
const c = [150, 132, 80, 40]
const d = [15, 26, 10, 23, 85]

const arraySortInfo = (array) => {
    if (array.some((element) => typeof element !== 'number')) {
        return 'array has strings'
    }

    if (array.every((element,index) => index > 0 ? element >= array[index - 1] : true)){
    return 'array is sorted by ascending'
    }

    if (array.every((element,index) => index > 0 ? element <= array[index - 1] : true)){
    return 'array is sorted by descending'
    }

    return 'array is not sorted'
}

console.log(arraySortInfo(a))
console.log(arraySortInfo(b))
console.log(arraySortInfo(c))
console.log(arraySortInfo(d))