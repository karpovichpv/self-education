const someString = 'Paul Karpovich'
const isSomeStringOrdinaryString = someString instanceof String

if (!isSomeStringOrdinaryString) {
    console.log(someString.toUpperCase())
}