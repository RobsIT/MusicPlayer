
const toggleButton = document.getElementById('toggleButton');
const audioPlayer = document.getElementById('audioPlayer');
const audioSource = document.getElementById("audioSource");
const progressBar = document.getElementById("progressBar");
const volumeControl = document.getElementById("volumeControl");

// Limit volume
audioPlayer.volume = 0.025;
// Listen for Volume changes and limit
audioPlayer.addEventListener('volumechange', () =>{
    if (audioPlayer.volume > 0.025)
    {
        audioPlayer.volume = 0.025; // Reset volume if it exceedes the limit
    }
});


// List of audio files
const playlistItems = Array.from(document.querySelectorAll("#playlist a"));
let currentIndex = 0;

// Play audio file
const playAudio = (filePath = null, songId = null) => {
    if (filePath)
    {
        console.log('Playing audio:', filePath, 'with songId:', songId); // Debugging
        audioSource.src = filePath;
        audioPlayer.load();
        toggleButton.classList.remove('bi-play-fill');
        toggleButton.classList.add('bi-pause-fill');  // Add pause icon
        
    }
    else if (!audioSource.src)
    {
        console.error('No audio file loaded to play.');
        return;
    }

    audioPlayer.play();
    // Sends the songId and runs the OnPostUpdateSongClicksAsync in Index.cshtml.cs
    if (songId) {
        fetch('/api/SongClicksApi/increment', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(songId)
        })
    }
};

// Play/Pause Button
let isPaused = false;
const pauseButton = document.querySelector("button[onclick='playPauseAudio()']");
toggleButton.addEventListener("click", () => {
    if (isPaused)
    {
        audioPlayer.play();
        toggleButton.classList.remove('bi-play-fill'); // Remove play icon
        toggleButton.classList.add('bi-pause-fill');  // Add pause icon
        isPaused = false;
    }
    else
    {
        audioPlayer.pause();
        toggleButton.classList.remove('bi-pause-fill'); // Remove pause icon
        toggleButton.classList.add('bi-play-fill');  // Add play icon
        isPaused = true;
    }
});

// Stop audio and go back to start
function stopAudio()
{
    audioPlayer.pause();
    audioPlayer.currentTime = 0;
    toggleButton.classList.remove('bi-pause-fill'); // Remove pause icon
    toggleButton.classList.add('bi-play-fill');  // Add play icon
}

// Play next song in playlist
function playNext()
{
    const playlistItems = Array.from(document.querySelectorAll(".cardPlaylistContent a[href^='#']"));

    if (playlistItems.length === 0)
    {
        console.error("No playlist items found.");
        return;
    }

    currentIndex = (currentIndex + 1) % playlistItems.length;

    const nextItem = playlistItems[currentIndex];
    const nextFilePath = nextItem.getAttribute("onclick").match(/'([^']+)'/)[1];

    playAudio(nextFilePath, nextItem.dataset.songId);
}

// Format time as mm:ss
function formatTime(seconds) {
    const minutes = Math.floor(seconds / 60);
    const secs = Math.floor(seconds % 60);
    return `${minutes}:${secs < 10 ? '0' : ''}${secs}`;
}

// Update play time
audioPlayer.addEventListener('timeupdate', () => {
    const currentTime = audioPlayer.currentTime;
    const duration = audioPlayer.duration || 0;
    timeDisplay.textContent = `${formatTime(currentTime)} / ${formatTime(duration)}`;

    // Update progressBar
    progressBar.value = (currentTime / duration) * 100 || 0;
});

// Update timeline
audioPlayer.addEventListener("timeupdate", () => {
    progressBar.value = (audioPlayer.currentTime / audioPlayer.duration) * 100;
});

// Jump around in track while playning
progressBar.addEventListener("input", () => {
    audioPlayer.currentTime = (progressBar.value / 100) * audioPlayer.duration;
});

// Adjust volume
volumeControl.addEventListener("input", () => {
    audioPlayer.volume = volumeControl.value;
});

// When song is finnished, play next song
audioPlayer.addEventListener("ended", playNext);

