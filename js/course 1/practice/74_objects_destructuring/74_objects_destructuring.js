const person1 = {
    name: 'Mike',
    info: {
        country: 'Spain',
        age: 23,
    },
    postQty: 100
}

const person2 = {
    name: 'Alice',
    info: {
        country: 'Italy',
        age: 25,
    },
}

const shortPerson = (person) => {
    const { name: n, info: { country: c, age: a }, postQty: p = 0 } = person
    return {
        n,
        c,
        a,
        p
    }
}

console.log(shortPerson(person1))
console.log(shortPerson(person2))