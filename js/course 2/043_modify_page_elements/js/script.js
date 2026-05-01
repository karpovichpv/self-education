'use strict';

const box = document.getElementById('box');
const btns = document.getElementsByTagName('button')
const circles = document.getElementsByClassName('circle')
const hearts = document.querySelectorAll('.heart');
const oneHeart = document.querySelector('.heart');
const wrapper = document.querySelector('.wrapper');

console.dir(box);
box.style.backgroundColor = 'blue'
box.style.backgroundColor = '500px'

btns[1].style.borderRadius = '100%';
circles[0].style.backgroundColor = 'red'

box.style.cssText = 'background-color: blue; width: 500px'
console.log(hearts);

hearts.forEach(i => i.style.backgroundColor = 'blue')

const div = document.createElement('div')

div.classList.add('black')
wrapper.append(div);
wrapper.prepend(div);

hearts[0].after(div);

circles[0].remove();
hearts[0].replaceWith(circles[0]);

div.innerHTML = '<h1>Hello World</h1>';

//div.textContent = 'Hello'

div.insertAdjacentHTML('beforebegin', '<h2>BeforeBegin</h2>');
div.insertAdjacentHTML('afterbegin', '<h2>AfterBegin</h2>');
div.insertAdjacentHTML('beforeEnd', '<h2>BeforeEnd</h2>');
div.insertAdjacentHTML('afterEnd', '<h2>AfterEnd</h2>');
