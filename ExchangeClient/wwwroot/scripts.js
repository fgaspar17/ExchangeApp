async function getGeolocation(text) {
    return new Promise((resolve, reject) => {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve({ longitud: position.coords.longitude, latitud: position.coords.latitude });
                },
                (error) => {
                    reject(error);
                }
            );
        } else {
            reject("Geolocation not available");
        }
    });
}
