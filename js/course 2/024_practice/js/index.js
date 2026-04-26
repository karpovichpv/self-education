const numberOfFilms = +prompt('How many films did you watch?', '');
const personalMovieDb = {
    count: numberOfFilms,
    movies: {},
    actors: {},
    genres: [],
    privat: false
};

for (let i = 0; i < numberOfFilms; i++) {
    const a = prompt('Write some of last films?', '');
    const b = prompt('How is it?', '');
    if (a != null && b != null && a != '' && b != '' && a.length < 50) {
        personalMovieDb.movies[a] = b;
        console.log('done');
    } else {
        console.log('error');
        i--;
    }
    personalMovieDb.movies[a] = b;
};

if (personalMovieDb.count < 10) {
} else if (personalMovieDb.count >= 10 && personalMovieDb.count < 30{
    console.log("You are a classical movie viewer")
} else if (personalMovieDb.count >= 10) {
    console.log("You are a binge watcher")
}


console.log(personalMovieDb);


