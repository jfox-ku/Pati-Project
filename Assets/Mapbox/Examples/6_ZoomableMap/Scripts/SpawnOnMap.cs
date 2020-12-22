namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		List<string> _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

        public void SetLocations(List<string> inp) {
            _locationStrings = inp;
        }

        //Can add to this function to give more functioanlity or customization to MapTags
        public void AddTagToLoc(string toAdd) {
            _locationStrings.Add(toAdd);
        }

        public void PlaceMapTags()
		{
			_locations = new Vector2d[_locationStrings.Count];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Count; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
                var MapTag = instance.GetComponent<MapTagDisplayScript>();
                MapTag.lat = float.Parse( _locations[i].x+"");
                MapTag.lon = float.Parse(_locations[i].y + "");

                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}
		}

		private void Update()
		{
            if (_spawnedObjects == null) return;

			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}

        public void UpdateAndPlaceTags(List<string> inp) {
            ResetTags();
            SetLocations(inp);
            PlaceMapTags();

        }


        

        private void ResetTags() {
            int i = 0;
            foreach(GameObject place in _spawnedObjects) {
                Destroy(place.gameObject);
                i++;
            }
            Debug.Log("Reset "+i+" MapTags.");
            _spawnedObjects.Clear();
        }

    }
}