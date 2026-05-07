'use strict';

const arr = ['Alex', 'Ann', 'Oleg', 'Alex']

const set = new Set(arr)

set.add('Ivan')
    .add('Oleg')
    .add('Ivan');

console.log(set);

// set.delete(value);
// set.has(value);
// set.clear();
// set.size;

for (let value of set)
    console.log(value);

set.forEach((value, valueAgain, set) => {
    console.log(value, valueAgain);
})

console.log(set.values());
console.log(set.keys());
console.log(set.entries());

function unique(arr) {
    return Array.from(new Set(arr))
}

console.log(unique(['ann', 'ann']));