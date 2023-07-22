class ExtendedArray extends Array{
    sum() {
        return this.reduce((sum, num) => sum + num, 0)
    }

    onlyNumbers() {
        return this.filter((el) => typeof el === 'number')
    }
}

const myExtendedArray = new ExtendedArray(10, 4, 5, 'true')
console.log(myExtendedArray)

console.log(myExtendedArray.sum())
console.log(myExtendedArray.onlyNumbers())