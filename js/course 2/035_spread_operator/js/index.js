"use strict";

let a = 6;
let b = a;

b += 5;
console.log(b);
console.log(a);


const obj = {
    a: 5,
    b: 1
};

copy.a = 10;
console.log(obj);

function copy(obj) {
    let newObj = {};
    let key;
    for (key in obj) {
        newObj[key] = obj[key];
    }

    return newObj;
}

const testObj = {
    a: 5,
    b: 1,
    c: {
        x: 7,
        y: 4,
    }
};

console.log(copy(testObj));

const add = {
    d: 17,
    e: 20
};

console.log(Object.assign(testObj, add));
var clonedElement = Object.assign(testObj, add);
clonedElement.d = 3330;

console.log(clonedElement);

const oldArray = ['a', 'b', 'c'];
const newArray = oldArray.slice();

console.log(newArray);
console.log(oldArray);

const video = ['youtube', 'vimeo', 'rutube'];
const blogs = ['wordpress', 'livejournal', 'blogger'];

const internet = [...video, ...blogs, 'vk', 'facebook'];
console.log(internet);

function log(a, b, c) {
    console.log(a);
    console.log(b);
    console.log(c);
}

const num = [2, 5, 7];

log(...num);

const array = ['a', 'b', 'c'];
const array1 = [...array];
console.log(array1);

const q = {
    one: 1,
    two: 2,
};

console.log({ ...q });