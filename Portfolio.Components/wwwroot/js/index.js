// VARIABLES
let lastKnownScrollY = 0;
let currentScrollY = 0;
let header = null;
const headerId = 'page-header';

const classes = {
    pinned: 'page-header--pinned',
    unpinned: 'page-header--unpinned',
    colored: 'page-header--colored'
};

// BLAZOR FUNCTIONS
window.blazorHelpers = {
    scrollToFragment: (elementId) => {
        var element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth'
            });
        }
    },
    lockBodyScroll: () => {
        var classList = document.body.classList;
        if (!classList.contains('is-locked')) {
            classList.add('is-locked');
        }
    },
    unlockBodyScroll: () => {
        var classList = document.body.classList;
        if (classList.contains('is-locked')) {
            classList.remove('is-locked');
        }
    },
    initScrollableHeader: () => {
        header = document.getElementById(headerId);
        document.addEventListener('scroll', onScroll, false);
    }
};

// OTHER JAVASCRIPT FUNCTIONS
function onScroll() {

    currentScrollY = window.pageYOffset
    currentScrollY > 0 ? colorHeader() : uncolorHeader();

    if (currentScrollY < lastKnownScrollY) {
        pinHeader();
    } else if (currentScrollY > lastKnownScrollY) {
        unpinHeader();
    }
    lastKnownScrollY = currentScrollY;
}

function pinHeader() {
    if (header.classList.contains(classes.unpinned)) {
        header.classList.remove(classes.unpinned)
        header.classList.add(classes.pinned)
    }
}

function unpinHeader() {
    if (header.classList.contains(classes.pinned) || !header.classList.contains(classes.unpinned)) {
        header.classList.remove(classes.pinned);
        header.classList.add(classes.unpinned);
    }
}

function colorHeader() {
    if (!header.classList.contains(classes.colored)) {
        header.classList.add(classes.colored);
    }
}

function uncolorHeader() {
    if (header.classList.contains(classes.colored)) {
        header.classList.remove(classes.colored);
    }
}