const obj = {
    key1: true,
    key5: 10,
    key3: 'abc',
    key4: null,
    key10: NaN,
}

const objKeys = Object.keys(obj)
objKeys.forEach((element) => {
    if (element === 'key1') {
        console.log(obj[element])
    }
});