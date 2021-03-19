using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XmlCrawler
{
    public class XmlReader
    {
        private string m_path;
        public XmlReader(string path)
        {
            Console.WriteLine("Enter path to ship files:");
            m_path = Console.ReadLine();

            var files = Directory.GetFiles(m_path, "*.xml", SearchOption.AllDirectories);

            if (!Directory.Exists(@"..\loadoutmod"))
                Directory.CreateDirectory(@"..\loadoutmod");

            //using (StreamWriter sw = new StreamWriter(@"..\loadoutmod\output.xml"))
            {
                foreach (var file in files)
                {
                    if (!file.Contains("macro") && !file.Contains("storage"))
                    {
                        string fileName = file.Split("\\").Last().Replace(".xml", "_macro.xml");

                        using (StreamWriter sw = new StreamWriter(@"..\loadoutmod\" + fileName))
                        {

                            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            sw.WriteLine("<diff>");
                            sw.WriteLine("<add sel=\"//macros/macro/properties\">");

                            sw.WriteLine("<loadouts>");
                            sw.WriteLine("\t<loadout id=\"default\">");
                            sw.WriteLine("\t\t<macros>");

                            string countermeasures = "countermeasure_flares_01_macro";
                            string drones = "ship_gen_s_fightingdrone_01_a_macro";
                            string repairdrones = "ship_gen_xs_repairdrone_01_a_macro";

                            string thrusterExtraLarge = "thruster_gen_xl_allround_01_mk1_macro";
                            string thruster = "thruster_gen_l_allround_01_mk1_macro";
                            //for argons:
                            string largeTurret = "turret_arg_l_laser_01_mk1_macro";
                            string mediumTurret = "turret_arg_m_gatling_02_mk1_macro";
                            string mediumShield = "shield_arg_m_standard_02_mk1_macro";
                            string largeShield = "shield_arg_l_standard_01_mk1_macro";
                            string extraLargeShield = "shield_arg_xl_standard_01_mk1_macro";
                            string engineLarge = "engine_arg_l_allround_01_mk1_macro";
                            string engineExtraLarge = "engine_arg_xl_allround_01_mk1_macro";
                            string largeWeapon = "weapon_arg_l_destroyer_01_mk1_macro";

                            //paranid
                            if (file.Contains("_par_") || file.Contains("canteran") || file.Contains("omicron") || file.Contains("split") || file.Contains("xl_cargo_hauler_3") || file.Contains("sucellus") || file.Contains("_ter_"))
                            {
                                largeTurret = "turret_par_l_laser_01_mk1_macro";
                                mediumTurret = "turret_par_m_gatling_02_mk1_macro";
                                mediumShield = "shield_par_m_standard_02_mk1_macro";
                                largeShield = "shield_par_l_standard_01_mk1_macro";
                                extraLargeShield = "shield_par_xl_standard_01_mk1_macro";
                                engineLarge = "engine_par_l_allround_01_mk1_macro";
                                engineExtraLarge = "engine_par_xl_allround_01_mk1_macro";
                                largeWeapon = "weapon_par_l_destroyer_01_mk1_macro";
                            }
                            //teladi
                            if (file.Contains("_tel_") || file.Contains("ions_collector"))
                            {
                                largeTurret = "turret_tel_l_laser_01_mk1_macro";
                                mediumTurret = "turret_tel_m_beam_02_mk1_macro";
                                mediumShield = "shield_tel_m_standard_02_mk1_macro";
                                largeShield = "shield_tel_l_standard_01_mk1_macro";
                                extraLargeShield = "shield_tel_xl_standard_01_mk1_macro";
                                engineLarge = "engine_tel_l_allround_01_mk1_macro";
                                engineExtraLarge = "engine_tel_xl_allround_01_mk1_macro";
                                largeWeapon = "weapon_tel_l_destroyer_01_mk1_macro";
                            }


                            var doc = XDocument.Load(file);
                            var shieldsExtralarge = doc.Descendants("connection").Where(x => (x.Attribute("tags") != null && x.Attribute("tags").Value.Contains("shield") && x.Attribute("tags").Value.Contains("extralarge"))).ToList();
                            foreach (var item in shieldsExtralarge)
                            {
                                sw.WriteLine("\t\t\t<shield macro=\"" + extraLargeShield + "\" path=\"../" + item.Attribute("name").Value + "\" />");
                            }
                            // internal shield generators
                            sw.WriteLine("\t\t\t<shield macro=\"" + "ishield_" + fileName.Replace(".xml", "") + "\" path=\".. / con_ishield_01\" exact=\"1\" optional=\"0\" />");
                            var shieldsLarge = doc.Descendants("connection").Where(x => (x.Attribute("tags") != null && x.Attribute("tags").Value.Contains("shield") && x.Attribute("tags").Value.Contains("large") && !x.Attribute("tags").Value.Contains("extralarge") && x.Attribute("group") == null)).ToList();
                            foreach (var item in shieldsLarge)
                            {
                                sw.WriteLine("\t\t\t<shield macro=\"" + largeShield + "\" path=\"../" + item.Attribute("name").Value + "\" />");
                            }
                            var enginesLarge = doc.Descendants("connection").Where(x => (x.Attribute("tags") != null && x.Attribute("tags").Value.Contains("engine") && x.Attribute("tags").Value.Contains("large") && !x.Attribute("tags").Value.Contains("extralarge"))).ToList();
                            foreach (var item in enginesLarge)
                            {
                                sw.WriteLine("\t\t\t<engine macro=\"" + engineLarge + "\" path=\"../" + item.Attribute("name").Value + "\" />");
                            }
                            var enginesExtraLarge = doc.Descendants("connection").Where(x => (x.Attribute("tags") != null && x.Attribute("tags").Value.Contains("engine") && x.Attribute("tags").Value.Contains("extralarge"))).ToList();
                            foreach (var item in enginesExtraLarge)
                            {
                                sw.WriteLine("\t\t\t<engine macro=\"" + engineExtraLarge + "\" path=\"../" + item.Attribute("name").Value + "\" />");
                                thruster = thrusterExtraLarge;
                            }

                            var weaponsLarge = doc.Descendants("connection").Where(x => (x.Attribute("tags") != null && x.Attribute("tags").Value.Contains("weapon") && x.Attribute("tags").Value.Contains("large"))).ToList();
                            foreach (var item in weaponsLarge)
                            {
                                sw.WriteLine("\t\t\t<weapon macro=\"" + largeWeapon + "\" path=\"../" + item.Attribute("name").Value + "\" />");
                            }

                            sw.WriteLine("\t\t</macros>");




                            // now the groups
                            //var shieldLargeGroup = doc.Descendants("connection").Where(x => (x.Attribute("tags") != null && x.Attribute("tags").Value.Contains("shield") && x.Attribute("tags").Value.Contains("large"))).ToList();

                            var ShieldLargeGroups = (from e in doc.Root.Elements("component").Elements("connections").Elements("connection")
                                                     select e).GroupBy(x => x.Attribute("group")).Select(x => x.First());

                            var disctingtGroups = GetDistinctlist(ShieldLargeGroups.ToList());

                            sw.WriteLine("\t\t<groups>");

                            foreach (var item in disctingtGroups)
                            {
                                if (item.Item1.Attribute("tags").Value.Contains("shield") && item.Item1.Attribute("tags").Value.Contains("large") && !item.Item1.Attribute("tags").Value.Contains("extralarge"))
                                {
                                    //sw.WriteLine("\t\t\t<shields macro=\"" + largeShield + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" min=\"" + 0 + "\" max=\"" + item.Item2 + "\" optional=\"1\" />");
                                    sw.WriteLine("\t\t\t<shields macro=\"" + largeShield + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" exact=\"" + item.Item2 + "\" optional=\"1\" />");
                                }
                                if (item.Item1.Attribute("tags").Value.Contains("shield") && item.Item1.Attribute("tags").Value.Contains("medium"))
                                {
                                    //sw.WriteLine("\t\t\t<shields macro=\"" + mediumShield + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" min=\"" + 0 + "\" max=\"" + item.Item2 + "\" optional=\"1\" />");
                                    sw.WriteLine("\t\t\t<shields macro=\"" + mediumShield + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" exact=\"" + item.Item2 + "\" optional=\"1\" />");
                                }
                                if (item.Item1.Attribute("tags").Value.Contains("turret") && item.Item1.Attribute("tags").Value.Contains("large"))
                                {
                                    //sw.WriteLine("\t\t\t<turrets macro=\"" + largeTurret + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" min=\"" + 0 + "\" max=\"" + item.Item2 + "\" optional=\"1\" />");
                                    sw.WriteLine("\t\t\t<turrets macro=\"" + largeTurret + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" exact=\"" + item.Item2 + "\" optional=\"1\" />");
                                }
                                if (item.Item1.Attribute("tags").Value.Contains("turret") && item.Item1.Attribute("tags").Value.Contains("medium"))
                                {
                                    //sw.WriteLine("\t\t\t<turrets macro=\"" + mediumTurret + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" min=\"" + 0 + "\" max=\"" + item.Item2 + "\" optional=\"1\" />");
                                    sw.WriteLine("\t\t\t<turrets macro=\"" + mediumTurret + "\" path=\"..\" group=\"" + item.Item1.Attribute("group").Value + "\" exact=\"" + item.Item2 + "\" optional=\"1\" />");
                                }
                            }
                            sw.WriteLine("\t\t</groups>");

                            sw.WriteLine("\t\t<ammunition>");
                            sw.WriteLine("\t\t\t<ammunition macro=\"" + countermeasures + "\" min=\"" + 10 + "\" max=\"" + 50 + "\" optional=\"1\" />");
                            sw.WriteLine("\t\t\t<unit macro=\"" + drones + "\" exact=\"" + 10 + "\" optional=\"1\" />");
                            sw.WriteLine("\t\t\t<unit macro=\"" + repairdrones + "\" exact=\"" + 4 + "\" optional=\"1\" />");
                            sw.WriteLine("\t\t</ammunition>");
                            sw.WriteLine("\t\t<virtualmacros>");
                            sw.WriteLine("\t\t\t<thruster macro=\"" + thruster + "\" />");
                            sw.WriteLine("\t\t</virtualmacros>");

                            sw.WriteLine("\t</loadout>");
                            sw.WriteLine("</loadouts>");

                            sw.WriteLine("</add>");
                            sw.WriteLine("</diff>");
                        }
                    }
                }
            }

        }
        private List<Tuple<XElement, int>> GetDistinctlist(List<XElement> list)
        {
            List<Tuple<XElement, int>> result = new List<Tuple<XElement, int>>();

            foreach (var item in list.Where(c => c.Attribute("group") != null).Select(c => c))
            {
                if (result.Where(c => item.Attribute("group") != null && c.Item1.Attribute("group").Value == item.Attribute("group").Value && c.Item1.Attribute("tags").Value == item.Attribute("tags").Value).FirstOrDefault() == null)
                {
                    int count = list.Where(x => x.Attribute("group") != null && x.Attribute("group").Value == item.Attribute("group").Value && x.Attribute("tags").Value == item.Attribute("tags").Value).Count();
                    result.Add(Tuple.Create(item, count));
                }
            }
            //foreach (var item in result)
            //{
            //    int count = list.Where(x => x.Attribute("group").Value == item.Attribute("group").Value).Count();
            //}

            return result;
        }
    }
}
