const MIN = 1000
const MAX = 9999

const array = [1030, 2038, 5555, 1235, 9993, 7649]

const randomNumberGetter = () => {
    return Math.floor(Math.random()*((MAX - MIN + 1) + MIN))
}

const hasRandomNumber = (randomNumber) => {
    for (let i = 0; i < array.length; i++) {
        if (array[i] === randomNumber) {
            return true
        }
    }
    return false
}

let randomNumber = randomNumberGetter()

while (hasRandomNumber(randomNumber)) {
    array.push(randomNumberGetter())
}

console.log(array)