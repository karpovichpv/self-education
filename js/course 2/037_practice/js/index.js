'use strict'

const personalMovieDb = {
    count: 0,
    movies: {},
    actors: {},
    genres: [],
    privat: false,
    start: () => {
        personalMovieDb.count = +prompt('How many films did you watch?', '');

        while (personalMovieDb.count == '' || personalMovieDb.count == null || isNaN(personalMovieDb.count)) {
            personalMovieDb.count = +prompt('How many films did you watch?', '');
        }
    },
    rememberMyFilms: () => {
        for (let i = 0; i < personalMovieDb.count; i++) {
            const a = prompt('Write some of last films?', '').trim();
            const b = prompt('How is it?', '').trim();
            if (a != null && b != null && a != '' && b != '' && a.length < 50) {
                personalMovieDb.movies[a] = b;
                console.log('done');
            } else {
                console.log('error');
                i--;
            }
            personalMovieDb.movies[a] = b;
        };
    },
    detectPersonalLevel: () => {
        if (personalMovieDb.count < 10) {
            console.log("You watch not many films");
        } else if (personalMovieDb.count >= 10 && personalMovieDb.count < 30) {
            console.log("You are a classical movie viewer");
        } else if (personalMovieDb.count >= 10) {
            console.log("You are a binge watcher");
        }
    },
    showMyDb: (hidden) => {
        if (!hidden) {
            console.log(personalMovieDb);
        }
    },
    toggleVisibleMyDb: () => {
        if (personalMovieDb.privat) {
            personalMovieDb.privat = false
        } else {
            personalMovieDb.privat = true
        }
    },
    writeYourGeners: () => {
        for (let i = 1; i <= 3; i++) {
            let genre = prompt(`Ваш любимый жанр под номером ${i}`)
            if (genre === '' || genre == null) {
                console.log('Вы ввели некорректную функцию')
                i--;
            } else {
                personalMovieDb.genres[i - 1] = prompt("What is your favourite genre?");
            }

            personalMovieDb.genres.forEach((item, i)=>{
                console.log(`Любимый жанр ${i+1} - это ${item}`)
            })
        }
    }
}