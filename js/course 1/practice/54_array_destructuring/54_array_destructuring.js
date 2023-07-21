const processQuantities = (array) => {
    const [minQty, maxQty, defaultQty = 0] = array
    return defaultQty + maxQty - minQty
}

const inputQuantities1 = [8, 29, 10]
console.log(processQuantities(inputQuantities1))

const inputQuantities2 = [8, 29]
console.log(processQuantities(inputQuantities2))