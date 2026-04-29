"use strict";

let str = 'some string';
let strObj = new String(str);

//console.log(str);
//console.log(strObj);
//console.dir([1, 2, 3]);

const soldier = {
    health: 400,
    armor: 100,
    sayHello: function () {
        console.log("hello!");
    }
};

const john = {
    health: 100
};

john.__proto__ = soldier;

const paul = Object.create(soldier);

console.log(john.armor);
console.log(john.sayHello());

paul.sayHello();