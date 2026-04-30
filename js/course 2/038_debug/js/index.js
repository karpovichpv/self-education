'use strict';

function hello() {
    console.log('Say hello!');
    debugger;
}

hello();

function hi() {
    console.log('Say hi!');
}

hi();

const arr = [1, 125, 33, 55];
const sorted = arr.sort(compareNum);

function compareNum(a, b) {
    return a - b;
}

console.log(sorted);