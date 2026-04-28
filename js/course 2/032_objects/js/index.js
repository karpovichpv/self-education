"use strict";

const obj = {
    name: 'test',
    width: 1024,
    height: 1024,
    colors: {
        border: 'black',
        bg: 'red'
    },
    makeTest: function () {
        console.log('Test');
    }
};
obj.makeTest();
const { border, bg } = obj.colors;
console.log(`border ${border}`);
console.log(`bg ${bg}`);

console.log(obj.name);

delete obj.name;
console.log(obj);

let counter = 0;
for (let key in obj) {
    if (typeof (obj[key]) === 'object') {
        for (let i in obj[key]) {
            console.log(`Property ${i} and has value ${obj[key][i]}`);
            counter++;
        }
    } else {
        console.log(`Property ${key} and has value ${obj[key]}`);
        counter++;
    }
}

console.log(counter);

console.log(Object.keys(obj).length);