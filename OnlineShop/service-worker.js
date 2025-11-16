const CACHE_NAME = "pwa-cache-v1";

// List files to cache
const FILES_TO_CACHE = [
    "/",
    "/css/site.css",
    "/js/site.js",
    "/Image",
    "/favicon.ico"
];

self.addEventListener("install", event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => cache.addAll(FILES_TO_CACHE))
    );
});

self.addEventListener("fetch", event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => response || fetch(event.request))
    );
});
