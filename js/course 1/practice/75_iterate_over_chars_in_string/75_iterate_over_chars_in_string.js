let vowelCount = 0
const vowels = ['a', 'e', 'i', 'o', 'u']

const str = 'Today is the besh day of my life'

const chars = str.split('')

chars.forEach((el) => {
    if (vowels.includes(el))
    vowelCount++
})

console.log(vowelCount)