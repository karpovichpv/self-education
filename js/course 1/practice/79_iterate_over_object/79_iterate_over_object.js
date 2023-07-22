const objectWithNumbers = {
    a: 10,
    b: 20,
    c: 'string',
    d: 12,
}

function sumObjectValues(nums) {
    let sum = 0
    Object.keys(nums).forEach((key) => {
        if (typeof nums[key] === 'number')
            sum += nums[key]
    })
    return sum
}

console.log(sumObjectValues(objectWithNumbers))