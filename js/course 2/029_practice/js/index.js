let numberOfFilms;

function start() {
    numberOfFilms = +prompt('How many films did you watch?', '');

    while (numberOfFilms == '' || numberOfFilms == null || isNaN(numberOfFilms)) {
        numberOfFilms = +prompt('How many films did you watch?', '');
    }
}

start();

const personalMovieDb = {
    count: numberOfFilms,
    movies: {},
    actors: {},
    genres: [],
    privat: false
};

function rememberMyFilms() {
    for (let i = 0; i < numberOfFilms; i++) {
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
}

//rememberMyFilms();

function detectPersonalLevel() {
    if (personalMovieDb.count < 10) {
        console.log("You watch not many films");
    } else if (personalMovieDb.count >= 10 && personalMovieDb.count < 30) {
        console.log("You are a classical movie viewer");
    } else if (personalMovieDb.count >= 10) {
        console.log("You are a binge watcher");
    }
}

//detectPersonalLevel();

function showMyDb(hidden) {
    if (!hidden) {
        console.log(personalMovieDb);
    }
}

showMyDb(personalMovieDb.privat);

function writeYourGeners() {
    for (let i = 1; i <= 3; i++) {
        personalMovieDb.genres[i - 1] = prompt("What is your favourite genre?");
    }
};

writeYourGeners();