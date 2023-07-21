let min, max

function minMax(...nums) {
    return [Math.min(...nums), Math.max(...nums)]
}

[min, max] = minMax(25, 4, 34, 10)
console.log(min, max);

[min, max] = minMax(18, 23, 103, 70, 80, 25)
console.log(min, max);