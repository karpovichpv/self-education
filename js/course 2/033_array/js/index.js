"use strict";

const arr = [0, 1, 2, 3, 6, 8];
arr.pop();
arr.push(10);
console.log(arr);

for (let value of arr) {
    console.log(value);
}


arr.forEach(function (item, i, arr) {
    console.log(`${i}: ${item} in the array ${arr}`);
});
