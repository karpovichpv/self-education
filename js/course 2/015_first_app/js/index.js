const numberOfFilms = +prompt('How many films did you watch?', '')
const personalMovieDb = {
    count: numberOfFilms,
    movies: {},
    actors: {},
    genres: [],
    private: false
}

const a = prompt('Write some of last films?', ''),
    b = prompt('How is it?', ''),
    c = prompt('Write some of last films?', ''),
    d = prompt('How is it?', '');

personalMovieDb.movies[a] = b
personalMovieDb.movies[c] = d


console.log(personalMovieDb)



