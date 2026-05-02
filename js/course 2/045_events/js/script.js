'use strict'

const btn = document.querySelector('button'),
    btns = document.querySelectorAll('button'),
    overlay = document.querySelector('.overlay');

//btn.onclick = function () {
//    alert('Click')
//}

btn.addEventListener('click', () => {
});


let i = 0;
function deleteElement() {
    return (e) => {
        console.log(e.currentTarget);
        console.log(e.type);
        //i++
        //if (i == 1) {
        //    btn.removeEventListener('click', deleteElement())
        //}
    };
}

btns.forEach(element => {
    element.addEventListener('click', deleteElement)
});

//btn.addEventListener('click', deleteElement());
//overlay.addEventListener('click', deleteElement());
//btn.removeEventListener('click', deleteElement());

const link = document.querySelector('a');

//link.addEventListener('click', (event) => {
//    event.preventDefault();

//    console.log(e.target)
//})
