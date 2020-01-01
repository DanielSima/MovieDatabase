var testmovie = [{
    "title": "The Avengers: Infinity War",
    "description": "Donec ut mauris eget massa tempor convallis. Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh. Quisque id justo sit amet sapien dignissim vestibulum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla dapibus dolor vel est. Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.",
    "release_date": "2019-08-03",
    "runtime": 104,
    "rating": 8.5,
    "poster_path": "http://dummyimage.com/500x750.bmp/5fa2dd/ffffff",
    "budget": 480031700,
    "genre": [
        {
            "title": "Animation"
        },
        {
            "title": "Action"
        },
        {
            "title": "Animation"
        },
        {
            "title": "Action"
        }
    ],
    "director": [
        {
            "name": "Lula Taillant"
        }
    ],
    "actor": [
        {
            "name": "Lavinia Ourry",
            "photo_path": "http://dummyimage.com/500x750.png/ff4444/ffffff",
            "character": "2019-06-04"
        },
        {
            "name": "Audy Tramel",
            "photo_path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff",
            "character": "2019-06-14"
        },
        {
            "name": "Krispin McGinny",
            "photo_path": "http://dummyimage.com/500x750.bmp/ff4444/ffffff",
            "character": "2019-05-02"
        },
        {
            "name": "Lawton Porson",
            "photo_path": "http://dummyimage.com/500x750.bmp/5fa2dd/ffffff",
            "character": "2019-09-07"
        },
        {
            "name": "Brittaney Drakard",
            "photo_path": "http://dummyimage.com/500x750.bmp/dddddd/000000",
            "character": "2019-05-09"
        },
        {
            "name": "Edwin Emmot",
            "photo_path": "http://dummyimage.com/500x750.png/dddddd/000000",
            "character": "2019-11-05"
        },
        {
            "name": "Dedie Jahan",
            "photo_path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff",
            "character": "2019-09-08"
        },
        {
            "name": "Avis Mulliner",
            "photo_path": "http://dummyimage.com/500x750.bmp/cc0000/ffffff",
            "character": "2019-05-19"
        },
        {
            "name": "Nikolos Caren",
            "photo_path": "http://dummyimage.com/500x750.png/cc0000/ffffff",
            "character": "2019-02-04"
        },
        {
            "name": "Harwilll Skeffington",
            "photo_path": "http://dummyimage.com/500x750.jpg/dddddd/000000",
            "character": "2019-12-18"
        }
    ],
    "language": [
        {
            "title": "Korean"
        },
        {
            "title": "Māori"
        },
        {
            "title": "Tajik"
        },
        {
            "title": "Somali"
        }
    ],
    "country": [
        {
            "title": "Thailand"
        },
        {
            "title": "Kenya"
        }
    ],
    "review": [
        {
            "author": "gwaren0",
            "description": "Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis. Sed ante. Vivamus tortor. Duis mattis egestas metus.",
            "date_created": "2019-10-13"
        },
        {
            "author": "dditer1",
            "description": "Proin risus. Praesent lectus. Vestibulum quam sapien, varius ut, blandit non, interdum in, ante. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio. Curabitur convallis. Duis consequat dui nec nisi volutpat eleifend.",
            "date_created": "2019-12-04"
        },
        {
            "author": "gfuentes2",
            "description": "Morbi ut odio. Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo. In blandit ultrices enim. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Proin interdum mauris non ligula pellentesque ultrices.",
            "date_created": "2019-09-22"
        }
    ]
}];


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
} else if (window.location.pathname == "/movie") {
    SpawnMovie();
    InitializeCarousel();
}


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
 * Adds or changes URL parameter and reloads the page.
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
        url: "/Index?handler=DefaultList",
        data: new URLSearchParams(window.location.search).toString(),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        //spawn movies
        for (i = 0; i < data.length; i++) {
            createMovieListChild(data[i].title, new Date(data[i].releaseDate).getFullYear(), data[i].rating, data[i].posterPath)
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
        createMovie(data[0]);
    }).fail(function (jqXHR, textStatus) {
        document.getElementById("parent").innerHTML = textStatus;
    })
}

/**
 * Creates HTML for movie on movie page.
 */
function createMovie(movie) {
    movie = movie[0];
    var parent = document.getElementById('parent');
    parent.children[0].children[0].src = movie.poster_path;
    var p = parent.children[0].children[1];
    p.children[0].innerHTML = movie.title;
    p.children[1].innerHTML = $.map(movie.genre, function (v) {
        return v.title;
    }).join(' | ');
    p.children[2].innerHTML = "&#9733 " + movie.rating;
    p.children[3].innerHTML = movie.description;
    p.children[4].innerHTML = "Released: " + new Date(movie.release_date).toLocaleDateString();
    p.children[5].innerHTML = movie.runtime + " min";
    p.children[6].innerHTML = "Budget: $" + (movie.budget / 100).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    p.children[7].innerHTML = "Directed by: " + $.map(movie.director, function (v) {
        return v.name;
    }).join(', ');
    var p = parent.children[1];
    for (var i = 0; i < movie.actor.length; i++) {
        actor = movie.actor[i];
        createActorForMovie(actor.name, actor.character, actor.photo_path);
    }

}

/**
 * Helper for create movie, creates an actor.
 */
function createActorForMovie(name, character, photo_path) {
    var html = ['<div class="actor">',
        '<img class="actor-poster list_poster img-fluid" src="' + photo_path + '" />',
        '<h5 class="actor-text list_poster_h1 text-truncate">' + name + '</h5>',
        '<h6 class="actor-text text-muted">as ' + character + '</h6>',
        '</div>'].join('\n');
    var parent = document.getElementById('actor-list');
    parent.insertAdjacentHTML('beforeend', html);
}


function InitializeCarousel() {
    $(document).ready(function () {
        $('.actor-list').slick({
            infinite: true,
            slidesToShow: 5,
            slidesToScroll: 1,
            responsive: [
                {
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
