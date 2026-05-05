'use strict';

const obj = {
    'name': 'Test',
}

let id = Symbol("id")

obj[id] = 1;
console.log(obj[id]);

