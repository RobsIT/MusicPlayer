const audioPlayer = document.getElementById('audioPlayer');
// Begränsa volymen till max 50%
audioPlayer.volume = 0.025;
// Lyssna på volymförändringar och begränsa till 50%
audioPlayer.addEventListener('volumechange', () => {
    if (audioPlayer.volume > 0.025) {
        audioPlayer.volume = 0.025; // Återställ om den går över gränsen
    }
});

// const audioPlayer = document.getElementById("audioPlayer");
const audioSource = document.getElementById("audioSource");
const progressBar = document.getElementById("progressBar");
const volumeControl = document.getElementById("volumeControl");

// Lista över alla ljudfiler
const playlistItems = Array.from(document.querySelectorAll("#playlist a"));
let currentIndex = 0;

// Spela upp en ljudfil
const playAudio = (filePath = null, songId = null) => {
    if (filePath) {
        console.log('Playing audio:', filePath, 'with songId:', songId); // Debugging
        audioSource.src = filePath;
        audioPlayer.load();
    } else if (!audioSource.src) {
        console.error('No audio file loaded to play.');
        return;
    }

    audioPlayer.play();

    if (songId) {
        fetch('/Index?handler=UpdateSongClicks', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ songId: songId })
        })
            .then(response => response.json())
            .then(data => {
                if (!data.success) {
                    console.error('Failed to update song clicks:', data.message);
                } else {
                    console.log('Song clicks updated successfully!');
                }
            });
    }
};


const pauseButton = document.querySelector("button[onclick='pauseAudio()']");
let isPaused = false;
// Pausa ljudet
pauseButton.addEventListener("click", () => {
    if (isPaused) {
        audioPlayer.play();
        pauseButton.textContent = "Pausa";
        isPaused = false;
    } else {
        audioPlayer.pause();
        pauseButton.textContent = "Spela";
        isPaused = true;
    }
});

// Stoppa ljudet och återställ tidslinjen
function stopAudio() {
    audioPlayer.pause();
    audioPlayer.currentTime = 0;
}

// Spela nästa låt i spellistan
function playNext() {
    const playlistItems = Array.from(document.querySelectorAll(".cardPlaylistContent a[href^='#']"));

    if (playlistItems.length === 0) {
        console.error("No playlist items found.");
        return;
    }

    currentIndex = (currentIndex + 1) % playlistItems.length;

    const nextItem = playlistItems[currentIndex];
    const nextFilePath = nextItem.getAttribute("onclick").match(/'([^']+)'/)[1];

    playAudio(nextFilePath, nextItem.dataset.songId);
}

// Uppdatera tidslinjen
audioPlayer.addEventListener("timeupdate", () => {
    progressBar.value = (audioPlayer.currentTime / audioPlayer.duration) * 100;
});

// Hoppa i låten när tidslinjen ändras
progressBar.addEventListener("input", () => {
    audioPlayer.currentTime = (progressBar.value / 100) * audioPlayer.duration;
});

// Justera volymen
volumeControl.addEventListener("input", () => {
    audioPlayer.volume = volumeControl.value;
});

// När låten är klar, spela nästa
audioPlayer.addEventListener("ended", playNext);

//<hr />
//<h6>Lägg in Filer i audio-mappen</h6> 
//<div>
//    <form method="post" enctype="multipart/form-data">
//        <input type="file" name="AudioFiles" accept=".mp3,.wav" multiple />
//        <button type="submit">Ladda upp filer</button>
//    </form>
//</div>
//<br/>
//<h5>Alla filer i audio-mappen</h5>
//<ul id="playlist">
//    @foreach (var file in Model.AudioFilesList)
//    {
//        if (file != null)
//        {
//            <li>
//                <a href="#" onclick="playAudio('@file')">@System.IO.Path.GetFileName(file)</a>
//            </li>
//        }
//    }
//</ul>
