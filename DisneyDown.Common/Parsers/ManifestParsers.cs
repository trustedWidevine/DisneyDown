﻿using DisneyDown.Common.Net;
using DisneyDown.Common.Parsers.HLS;
using DisneyDown.Common.Parsers.HLS.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InvertIf
// ReSharper disable RedundantIfElseBlock

namespace DisneyDown.Common.Parsers
{
    /// <summary>
    /// Contains generic methods for parsing content manifests
    /// </summary>
    public static class ManifestParsers
    {
        /// <summary>
        /// Verify if a manifest (master or content) is valid and conforms to the standard
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static bool ManifestValid(string playlist)
        {
            try
            {
                //validation
                if (!string.IsNullOrWhiteSpace(playlist))
                {
                    //parse
                    var p = new PlaylistParser().Parse(playlist);

                    //validation
                    if (p != null)
                    {
                        //additional validation
                        if (p.Items.Count > 0)
                        {
                            //get first tag
                            var tag = (PlaylistTagItem)p.Items[0];

                            //verify
                            return tag.Id == PlaylistTagId.EXTM3U;
                        }
                    }
                }
            }
            catch
            {
                //nothing
            }

            //default
            return false;
        }

        /// <summary>
        /// Download a manifest from a URL (contains additional verification than simply using ResourceGrab)
        /// </summary>
        /// <param name="playlistUrl"></param>
        /// <returns></returns>
        public static string DownloadManifest(string playlistUrl)
        {
            try
            {
                //validation
                if (!string.IsNullOrWhiteSpace(playlistUrl))
                {
                    //try playlist download
                    var playlist = ResourceGrab.GrabString(playlistUrl);

                    //validation
                    if (!string.IsNullOrWhiteSpace(playlist))

                        //return downloaded playlist
                        return playlist;
                }
                else
                    Console.WriteLine($"Incorrect content/playlist URL: {playlistUrl}");
            }
            catch (Exception ex)
            {
                //report error
                Console.WriteLine($"Playlist download error:\n\n{ex.Message}");
            }

            //default
            return @"";
        }

        private static bool ValidSegmentUrl(string urlSegment)
        {
            //check values for verification
            var checkValues = new[] { @"-BUMPER/", @"DUB_CARD" };

            //if there's any match, it's an instant false
            return checkValues.All(s => !urlSegment.Contains(s));
        }

        /// <summary>
        /// Lists all manifest MPEG-4 map URLs (MPEG-4 initialisation segment data)
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static string[] ManifestAllMapUrls(string playlist)
        {
            try
            {
                //validation
                if (!string.IsNullOrWhiteSpace(playlist))
                {
                    //attempt load
                    var p = new PlaylistParser().Parse(playlist);

                    //validation
                    if (p != null)
                    {
                        //playlist maps are stored temporarily
                        var mapList = new List<string>();

                        //go through each located tag
                        foreach (var i in p.Items)
                        {
                            try
                            {
                                //try casting to tag
                                var tag = (PlaylistTagItem)i;

                                //verify map
                                if (tag.Id == PlaylistTagId.EXT_X_MAP)
                                    //find URI attribute and verify it against the match criteria
                                    foreach (var a in tag.Attributes.Where(a
                                        => a.Key == @"URI"))
                                    {
                                        //assign the return value
                                        mapList.Add(a.Value);
                                        break;
                                    }
                            }
                            catch
                            {
                                //ignore
                            }
                        }

                        //return the result
                        return mapList.ToArray();
                    }
                    else
                        Console.WriteLine(@"Null playlist parse result; couldn't find map URL");
                }
                else
                    Console.WriteLine(@"Null or empty playlist supplied; couldn't find list of map URLs");
            }
            catch (Exception ex)
            {
                //report error
                Console.WriteLine($"Playlist parse error:\n\n{ex.Message}");
            }

            //default
            return null;
        }

        /// <summary>
        /// Fetches the manifest MPEG-4 map URL (MPEG-4 initialisation segment data)
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static string ManifestMainMapUrl(string playlist)
        {
            try
            {
                //validation
                if (!string.IsNullOrWhiteSpace(playlist))
                {
                    //get all map urls
                    var mapList = ManifestAllMapUrls(playlist);

                    //loop through each one and return the first correct match
                    foreach (var m in mapList)

                        //validate the URL
                        if (ValidSegmentUrl(m))

                            //return result if valid
                            return m;
                }
                else
                    Console.WriteLine(@"Null or empty playlist supplied; couldn't find map URL");
            }
            catch (Exception ex)
            {
                //report error
                Console.WriteLine($"Playlist parse error:\n\n{ex.Message}");
            }

            //default
            return @"";
        }

        /// <summary>
        /// Fetches the manifest MPEG-4 Disney+ intro (bumper) map URL (MPEG-4 initialisation segment data)
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static string ManifestBumperMapUrl(string playlist)
        {
            try
            {
                //validation
                if (!string.IsNullOrWhiteSpace(playlist))
                {
                    //get all map urls
                    var mapList = ManifestAllMapUrls(playlist);

                    //loop through each one and return the first correct match
                    foreach (var m in mapList)

                        //validate the URL
                        if (m.Contains(@"-BUMPER/"))

                            //return result if valid
                            return m;
                }
                else
                    Console.WriteLine(@"Null or empty playlist supplied; couldn't find Disney+ intro map URL");
            }
            catch (Exception ex)
            {
                //report error
                Console.WriteLine($"Playlist parse error:\n\n{ex.Message}");
            }

            //default
            return @"";
        }
    }
}