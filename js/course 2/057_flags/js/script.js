'use strict';

const user = {
    name: 'Alex',
    surname: 'Smith',
    birthday: '20/04/1995',
    showMyPublicData: () => {
        console.log(`${this.name} ${this.surname}`);
    }
}
//Object.defineProperty(user, 'birthday', { value: prompt('Date?'), writable: false, configurable: true })

console.log(Object.getOwnPropertyDescriptor(user, 'birthday'));

Object.defineProperty(user, 'name', { writable: false })
Object.defineProperty(user, 'gender', { value: 'male' })

//console.log(Object.getOwnPropertyDescriptor(user, 'gender'));


console.log(Object.getOwnPropertyDescriptor(Math, 'PI'));

Object.defineProperties(user, {
    name: { writable: false },
    surname: { writable: false }
})

Object.preventExtensions(user);
Object.seal(user);
Object.freeze(user);
