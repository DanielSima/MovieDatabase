// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function TestList() {
    $.ajax({
        type: "GET",
        url: '/Index?handler=TestList',
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        //TODO remove
        console.log(data);
        //remove spinner
        document.getElementById("spinner").parentNode.removeChild(spinner);
        for (i = 0; i < data.length; i++) {
            //spawn movies
            create_list_child(data[i].title, new Date(data[i].releaseDate).getFullYear(), data[i].rating, data[i].posterPath)
        }
    }).fail(function (jqXHR, textStatus) {
        document.getElementById("spinner").innerHTML = textStatus;
    })
}

TestList();

var movie = [{
    "title": "Sweetie",
    "year": 2000,
    "rating": 1.2,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "Moonlight Mile",
    "year": 1995,
    "rating": 3.7,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Exercice de l'État, L'",
    "year": 1996,
    "rating": 2.7,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "Restoration",
    "year": 1993,
    "rating": 2.4,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}, {
    "title": "Hippie Revolution, The",
    "year": 2013,
    "rating": 5.6,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Faust",
    "year": 2011,
    "rating": 9.1,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "People in Places",
    "year": 1990,
    "rating": 9.6,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Dante's Inferno",
    "year": 2006,
    "rating": 9.3,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "Casey Jones",
    "year": 1997,
    "rating": 8.3,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "Horror Business",
    "year": 2008,
    "rating": 7.6,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Snakes and Earrings (Hebi ni piasu)",
    "year": 1994,
    "rating": 3.0,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}, {
    "title": "Open Season",
    "year": 1999,
    "rating": 2.6,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "Low Down Dirty Shame, A",
    "year": 2009,
    "rating": 3.7,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Salaam-E-Ishq",
    "year": 1998,
    "rating": 2.1,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Why We Fight",
    "year": 2008,
    "rating": 5.2,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Little Brother, Big Trouble: A Christmas Adventure (Niko 2: Lentäjäveljekset)",
    "year": 1986,
    "rating": 9.4,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Setup (Set Up)",
    "year": 1989,
    "rating": 1.5,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Holiday",
    "year": 1998,
    "rating": 9.9,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}, {
    "title": "Blind Shaft (Mang jing)",
    "year": 1992,
    "rating": 1.8,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Geri's Game",
    "year": 2004,
    "rating": 1.8,
    "path": "http://dummyimage.com/500x750.jpg/dddddd/000000"
}, {
    "title": "Greystone Park",
    "year": 1990,
    "rating": 8.8,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Carbine Williams",
    "year": 2008,
    "rating": 3.3,
    "path": "http://dummyimage.com/500x750.jpg/5fa2dd/ffffff"
}, {
    "title": "Lights in the Dusk (Laitakaupungin valot)",
    "year": 1989,
    "rating": 2.4,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}, {
    "title": "Agatha",
    "year": 2011,
    "rating": 4.2,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Bad Moon",
    "year": 1986,
    "rating": 3.6,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}, {
    "title": "Serial Killer Culture",
    "year": 2000,
    "rating": 1.8,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Bandits",
    "year": 2008,
    "rating": 9.9,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Mother (Madeo)",
    "year": 2011,
    "rating": 9.7,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}, {
    "title": "Dead in the Water",
    "year": 2012,
    "rating": 9.4,
    "path": "http://dummyimage.com/500x750.jpg/cc0000/ffffff"
}, {
    "title": "Whatever",
    "year": 1998,
    "rating": 9.3,
    "path": "http://dummyimage.com/500x750.jpg/ff4444/ffffff"
}]
/*
for(i = 0; i< movie.length; i++){
    //spawn movies
    create_list_child(movie[i].title, movie[i].year, movie[i].rating, movie[i].path)
}*/


function create_list_child(title, year, rating, poster_url) {
    var html = ['<div class="list_child">',
        '<img class="list_poster img-fluid" alt="Responsive image" ', 'src="http://image.tmdb.org/t/p/w500//' + poster_url + '">',
            '<h5 class="list_poster_h1 text-truncate">' + title + '</h5>',
            '<h6 class="inline text-muted">' + year + '</h6>',
            '<h6 class="inline text-muted float-right">&#9733 ' + rating + '</h6>',
            '</div>'].join('\n');
    var parent = document.getElementById('list_parent');
    parent.insertAdjacentHTML('beforeend', html);
}