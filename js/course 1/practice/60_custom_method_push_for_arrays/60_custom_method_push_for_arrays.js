class CustomArray extends Array{
    customPush(newElement){
        console.log(this.length)
        this[this.length] = newElement
        console.log(this.length)

        console.log(`New element ${newElement} was added`)
    }
}

const myCustomArray = new CustomArray(10, 3, 7)
myCustomArray.customPush(25)

console.log(myCustomArray)