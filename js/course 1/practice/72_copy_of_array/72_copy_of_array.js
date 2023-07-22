const a = [1, 2, 3]
const b = [...a]
const c = Array.from(a)
const d = JSON.parse(JSON.stringify(a)) 

b.push('newElement')

console.log(a)

console.log(b)
console.log(c)
console.log(d)