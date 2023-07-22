const objectWithNumbers = {
    x: 5,
    y: 20,
    z: 3,
}

// Option 1
const mult = (obj) => {
    const { x, y, z } = obj
    return x * y * z
}

// Option 2
const mult2 = ({ x, y, z }) => x * y * z

const result = mult(objectWithNumbers)
console.log(result)