/* Module Script */
var GIBS = GIBS || {};

GIBS.TideCharts = {
    setDocumentTitle: (title) => {
        document.title = title;
    },

    initMap: (mapId, lat, lon, zoom, popupText, imagePath) => {
        var mapElement = document.getElementById(mapId);
        if (mapElement) {
            // Clear previous map instance if it exists to prevent errors on re-render
            if (mapElement._leaflet_id) {
                mapElement._leaflet_id = null;
                mapElement.innerHTML = ''; // Clear the div content
            }

            // Set the path for Leaflet's default icon images
            L.Icon.Default.imagePath = imagePath;

            // Initialize the map
            var map = L.map(mapId).setView([lat, lon], zoom);

            // Add the OpenStreetMap tile layer
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(map);

            // Add a marker for the station
            L.marker([lat, lon]).addTo(map)
                .bindPopup('Tide Station: ' + popupText)
                .openPopup();
        }
    },

    initChart: (canvasId, chartType, jsonData) => {
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        // To prevent errors on re-render, destroy the previous chart instance if it exists
        let existingChart = Chart.getChart(canvas);
        if (existingChart) {
            existingChart.destroy();
        }

        const data = JSON.parse(jsonData);

        // Prepare data for Chart.js
        const labels = data.map(p => p.label);
        const values = data.map(p => p.value);
        const fullDates = data.map(p => p.fullDate);
        const pointColors = data.map(p => p.type === 'H' ? 'rgba(255, 99, 132, 1)' : 'rgba(54, 162, 235, 1)');

        const chartConfig = {
            type: chartType,
            data: {
                labels: labels,
                datasets: [{
                    label: 'Tide Prediction',
                    data: values,
                    borderColor: chartType === 'line' ? 'rgba(75, 192, 192, 1)' : pointColors,
                    backgroundColor: chartType === 'line' ? 'rgba(75, 192, 192, 0.2)' : pointColors,
                    borderWidth: 1,
                    pointRadius: chartType === 'line' ? 5 : undefined,
                    pointBackgroundColor: chartType === 'line' ? pointColors : undefined,
                    pointBorderColor: chartType === 'line' ? '#fff' : undefined,
                    pointHoverRadius: chartType === 'line' ? 7 : undefined,
                    fill: chartType === 'line' ? false : undefined
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        title: { display: true, text: 'Date/Time' },
                        ticks: { autoSkip: true, maxTicksLimit: 10 }
                    },
                    y: {
                        beginAtZero: false,
                        title: { display: true, text: 'Water Level (ft)' }
                    }
                },
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const value = context.parsed.y.toFixed(2);
                                const index = context.dataIndex;
                                const type = data[index].type;
                                const date = fullDates[index];
                                const tideTypeLabel = type === 'H' ? 'High Tide' : 'Low Tide';
                                return [date, tideTypeLabel + ': ' + value + ' ft'];
                            }
                        }
                    }
                }
            }
        }; 

        new Chart(canvas, chartConfig);
    }
};