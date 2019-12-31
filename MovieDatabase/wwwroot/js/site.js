
//spawn movies
SpawnMovieList("DefaultList");
//add onclick to buttons

$(document).ready($(".dropdown-item").click(function () { AddParameter() }));
$(document).ready($(".btn_seach").click(function () { AddParameter(true) }));

$(document).ready($(".no-attr").unbind("click").bind("click", function () { RemoveParameter() }));


//$(document).ready($(".list_child").click(function () { OpenMovie() }));


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
function SpawnMovieList(handler = "DefaultList") {
    $.ajax({
        type: "GET",
        url: '/Index?handler=' + handler,
        data: new URLSearchParams(window.location.search).toString(),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        //spawn movies
        for (i = 0; i < data.length; i++) {
            createMovieListChild(data[i].title, new Date(data[i].releaseDate).getFullYear(), data[i].rating, data[i].posterPath)
        }
        //add onclick
        $(".list_child").click(function () { OpenMovie() })
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
 * Creates HTML for movie on movie page.
 */
function createMovie() {
    var html = ['<div class="list_child">',
        '<img class="list_poster img-fluid" alt="Responsive image" ', 'src="http://image.tmdb.org/t/p/w500//' + poster_url + '">',
        '<h5 class="list_poster_h1 text-truncate">' + title + '</h5>',
        '<h6 class="inline text-muted">' + year + '</h6>',
        '<h6 class="inline text-muted float-right">&#9733 ' + rating + '</h6>',
        '</div>'].join('\n');
    var parent = document.getElementById('list_parent');
    parent.insertAdjacentHTML('beforeend', html);
}