//if on default page
if (window.location.pathname == "/") {
    //spawn movies
    SpawnMovieList();
    //add onclick to buttons
    $(document).ready($(".dropdown-item").click(function () {
        AddParameter()
    }));
    $(document).ready($(".btn_seach").click(function () {
        AddParameter(true)
    }));
    //delete parameter when clicking 'all'
    $(document).ready($(".no-attr").unbind("click").bind("click", function () {
        RemoveParameter()
    }));
}
//if on movie page
else if (window.location.pathname == "/movie") {
    SpawnMovie();
}

///////////////////////////////
//FUNCTIONS
/**
 * Adds or changes URL parameter and reloads the page.
 */
function AddParameter(form = false) {
    var key = event.target.getAttribute("data-key");
    if (form == true) {
        var value = event.target.parentNode.children[0].children[0].value;
    } else {
        var value = event.target.innerHTML;
    }
    var searchParams = new URLSearchParams(window.location.search);
    searchParams.set(key, value);
    window.location.search = searchParams.toString();
}

/**
 * Removes URL parameter and reloads the page.
 */
function RemoveParameter() {
    var key = event.target.getAttribute("data-key");
    var searchParams = new URLSearchParams(window.location.search);
    searchParams.delete(key);
    window.location.search = searchParams.toString();
}

/**
 * Opens movie page with title as parameter.
 */
function OpenMovie() {
    var movieTitle = event.currentTarget.children[1].innerHTML;
    window.location.href = window.location.origin + "/movie?title=" + movieTitle;
}

/**
 * Requests movies from DB, then spawns them on list page.
 */
function SpawnMovieList() {
    $.ajax({
        type: "GET",
        url: "/Index?handler=List",
        data: new URLSearchParams(window.location.search).toString(),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        //spawn movies
        for (i = 0; i < data.length; i++) {
            createMovieListChild(data[i].title, new Date(data[i].releaseDate).getFullYear(), data[i].rating.toFixed(1), data[i].posterPath)
        }
        //add onclick
        $(".list_child").click(function () {
            OpenMovie()
        })
        //remove spinner
        document.getElementById("spinner").parentNode.removeChild(spinner);
    }).fail(function (jqXHR, textStatus) {
        document.getElementById("spinner").innerHTML = textStatus;
    })
}

/**
 * Creates HTML for movie on list page.
 */
function createMovieListChild(title, year, rating, poster_url) {
    var html = ['<div class="list_child">',
        '<img class="list_poster img-fluid" alt="Responsive image" ', 'src="http://image.tmdb.org/t/p/w500//' + poster_url + '">',
        '<h5 class="list_poster_h1 text-truncate">' + title + '</h5>',
        '<h6 class="inline text-muted">' + year + '</h6>',
        '<h6 class="inline text-muted float-right">&#9733 ' + rating + '</h6>',
        '</div>'].join('\n');
    var parent = document.getElementById('list_parent');
    parent.insertAdjacentHTML('beforeend', html);
}

/**
 * Requests movie from DB, then spawns it on movie page.
 */
function SpawnMovie() {
    $.ajax({
        type: "GET",
        url: "/Movie?handler=Movie",
        data: new URLSearchParams(window.location.search).toString(),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        //spawn movies
        createMovie(data);
    }).fail(function (jqXHR, textStatus) {
        document.getElementById("parent").innerHTML = textStatus;
    })
}

/**
 * Creates HTML for movie on movie page.
 */
function createMovie(movie) {
    movie = movie["movie"];
    var parent = document.getElementById('parent');
    parent.children[0].children[0].src = "http://image.tmdb.org/t/p/w500//" + movie.poster_path;
    var p = parent.children[0].children[1];
    p.children[0].innerHTML = movie.title;
    p.children[1].innerHTML = $.map(movie.genres, function (v) {
        return v.title;
    }).join(' | ');
    p.children[2].innerHTML = "&#9733 " + movie.rating.toFixed(1);
    p.children[3].innerHTML = movie.description;
    p.children[5].innerHTML = new Date(movie.release_date).toLocaleDateString();
    p.children[7].innerHTML = movie.runtime + " min";
    p.children[9].innerHTML = "$" + (movie.budget / 100).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    p.children[11].innerHTML = $.map(movie.directors, function (v) {
        return v.name;
    }).join(', ');
    var p = parent.children[1];
    for (var i = 0; i < movie.actors.length; i++) {
        actor = movie.actors[i];
        createActorForMovie(actor.name, actor.character, actor.photo_path);
    }
    InitializeCarousel();
    for (var i = 0; i < movie.reviews.length; i++) {
        review = movie.reviews[i];
        createReviewForMovie(review.author, review.description, review.date_created);
    }
}

/**
 * Helper for create movie, creates an actor.
 */
function createActorForMovie(name, character, photo_path) {
    var html = ['<div class="actor">',
        '<img class="actor-poster list_poster img-fluid" src="http://image.tmdb.org/t/p/w500//' + photo_path + '"/>',
        '<h5 class="actor-text list_poster_h1 text-truncate">' + name + '</h5>',
        '<h6 class="actor-text text-muted">as ' + character + '</h6>',
        '</div>'].join('\n');
    var parent = document.getElementById('actor-list');
    parent.insertAdjacentHTML('beforeend', html);
}

/**
 * Helper for create movie, creates a review.
 */
function createReviewForMovie(author, description, date_created) {
    var html = ['<div class="review movie-text">',
        '<h5 class="list_poster_h1 text-truncate inline">' + author + '</h5>',
        '<h5 class="list_poster_h1 text-truncate inline  text-muted"> - ' + new Date(date_created).toLocaleDateString() + '</h5>',
        '<h2 class="review-text text-justify font-weight-light">' + description + '</h2>',
        '</div>'].join('\n');
    var parent = document.getElementById('review-list');
    parent.insertAdjacentHTML('beforeend', html);
}

/**
 * Initializes actor carousel on lovie page.
 * */
function InitializeCarousel() {
    $(document).ready(function () {
        $('.actor-list').slick({
            infinite: true,
            slidesToShow: 6,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1600,
                    settings: {
                        slidesToShow: 5
                    }
                }, {
                    breakpoint: 1400,
                    settings: {
                        slidesToShow: 4
                    }
                },
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 1000,
                    settings: {
                        slidesToShow: 2
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                    }
                }
            ]
        });
    });
}